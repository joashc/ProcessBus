module Definitions where

import Data.Graph.Inductive
import Data.List
import Data.Function (on)
import Cyclicity
import qualified Data.Graph.Inductive.PatriciaTree as G

useError = flip maybe Right . Left

data ConfigError = DuplicateNodes | CyclicForwards | TransportDoesNotExist deriving Show

data TransportType = Bus | Queue deriving (Show, Eq)

data Transport = Transport {
  transportType :: TransportType,
  path :: String,
  forwards :: [Transport]
} deriving (Show)

type TransportConfig = [Transport]

instance Ord Transport where
  Transport _ p1 _ `compare` Transport _ p2 _ = compare p1 p2

instance Eq Transport where 
  Transport _ p1 _ == Transport _ p2 _ = p1 == p2

set :: Ord a => [a] -> Maybe [a]
set xs = if hasDupes then Nothing else Just xs 
  where hasDupes = (any $ (> 1) . length) . groupBy (==) . sort $ xs

configNodes :: TransportConfig -> [LNode Transport]
configNodes = zip ([1..] :: [Int])

forwardName :: Transport -> Transport -> String
forwardName a b = path a ++ ":" ++ path b

buildEdges :: [LNode Transport] -> Maybe [LEdge String]
buildEdges nodes 
  | null $ filter (not . null . forwards . snd) nodes = Just []
  | otherwise = sequence $ do
    lNode <- filter (not . null . forwards . snd) nodes
    forward <- forwards . snd $ lNode
    let mayFw = find ((== forward) . snd) nodes
    return $ do
      fwlNode <- mayFw
      return $ (fst lNode, fst fwlNode, forwardName' lNode fwlNode)
    where forwardName' = forwardName `on` snd

configGraph :: DynGraph g => TransportConfig -> Either ConfigError (g Transport String)
configGraph ts = do
  nonDupeConfig <- useError DuplicateNodes $ set ts
  let nodes = configNodes nonDupeConfig
  edges <- useError TransportDoesNotExist $ buildEdges nodes
  let graph = mkGraph nodes edges
  useError CyclicForwards $ acyclic graph

getEmpty x = do
  num <- x
  let plusOne = map (+ 1) num 
  return plusOne

