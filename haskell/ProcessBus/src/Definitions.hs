module Definitions where

data MessageTransport = Bus String | Queue String deriving (Show, Eq)

path :: MessageTransport -> String
path Bus p = p
path Queue p = p

data Forward = Forward {
	from :: MessageTransport,
	to :: MessageTransport
}

checkDuplicates :: Eq a => [a] -> Bool
checkDuplicates = any $ (> 1) . length . groupBy (==)