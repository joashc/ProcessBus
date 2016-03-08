module Definitions where

import Data.Graph.Inductive
import qualified Data.Graph.Inductive.PatriciaTree as G
import Data.List

data MessageTransport = Bus String | Queue String deriving (Show, Eq)

path :: MessageTransport -> String
path (Bus p) = p
path (Queue p) = p

data Forward = Forward {
	from :: MessageTransport,
	to :: MessageTransport
}

checkDuplicates :: Ord a => [a] -> Bool
checkDuplicates = (any $ (> 1) . length) . groupBy (==) . sort

-- | Checks if a node is a leaf node in a graph
isLeaf :: Graph g => g a b -> Node -> (Bool, Node)
isLeaf g node = (null $ suc g node, node)

-- | Checks if graph has any leaf nodes
hasLeaf :: Graph gr => gr a b -> Bool
hasLeaf gr = any fst . map (isLeaf gr) . nodes $ gr

-- | Delete an arbitrary leaf node from a graph
delLeaf :: DynGraph gr => gr a b -> Maybe (gr a b)
delLeaf gr = (flip delNode) gr <$> leaf
	where leaf = find isLeaf' $ nodes gr
	      isLeaf' = fst . isLeaf gr

-- | Checks if a graph is cyclic	      
-- Stricly speaking, we should never be able to return [Nothing];
-- the guards ensure that only graphs with leaf nodes reach the delete leaf case.
isCyclicSafe :: DynGraph gr => gr a b -> Maybe Bool
isCyclicSafe gr 
	| isEmpty gr = Just False
	| not $ hasLeaf gr = Just True
	| otherwise = delLeaf gr >>= isCyclicSafe

isCyclic :: DynGraph gr => gr a b -> Bool
isCyclic g = case isCyclicSafe g of
	Just x -> x
	Nothing -> error "Attempted to remove a leaf node where none existed."