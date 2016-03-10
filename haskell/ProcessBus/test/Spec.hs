import Test.QuickCheck
import Definitions
import qualified Data.Graph.Inductive.PatriciaTree as G

main :: IO ()
main = do
  ts <- sample' leafTransport
  f1 <- generate . listOf $ forwardN 1 ts
  print $ sequence (map configGraph f1 :: [Either ConfigError (G.Gr Transport String)])

instance Arbitrary TransportType where
  arbitrary = elements [Bus, Queue]

instance Arbitrary Transport where
  arbitrary = Transport <$> arbitrary <*> arbitrary <*> arbitrary

leafTransport :: Gen Transport
leafTransport = do 
  arbType <-arbitrary
  arbPath <- pathName
  return $ Transport arbType arbPath []

forward :: Transport -> Transport -> Transport
forward a b = a { forwards = b : forwards a }

forwardTransport :: [Transport] -> Gen [Transport]
forwardTransport ts = do
  from <- elements ts
  let withoutFrom = filter (/= from) ts
  to <- elements withoutFrom
  let fromF = forward from to
  return $ fromF : withoutFrom

pathName :: Gen String
pathName = do
  adjective <- elements adjectives
  noun <- elements nouns
  return $ adjective ++ " " ++ noun

forwardN :: Int -> [Transport] -> Gen [Transport]
forwardN n = iterateM n forwardTransport

iterateM :: Monad m =>Int ->  (a -> m a) -> a -> m a
iterateM 0 _ a = return a
iterateM n f a = f a >>= iterateM (n - 1) f

nouns :: [String]
nouns = [ "time", "year", "people", "way", "day", "man", "thing", "woman", "life", "child", "world", "school", "state", "family", "student", "group", "country", "problem", "hand", "part", "place", "week", "company", "system", "program", "question", "work", "government", "number", "night", "point", "home", "water", "room", "mother", "area", "money", "story", "fact", "month", "lot", "right", "study", "book", "eye", "job", "word", "business", "issue", "side", "kind", "head", "house", "service", "friend", "father", "power", "hour", "game", "line", "end", "member", "law", "car", "city", "community", "name", "president", "team", "minute", "idea", "kid", "body", "information", "back", "parent", "face", "others", "level", "office", "door", "health", "person", "art", "war", "history", "party", "result", "change", "morning", "reason", "research", "girl", "guy", "moment", "air", "teacher", "force", "education" ]

adjectives :: [String]
adjectives = [ "other", "new", "good", "high", "old", "great", "big", "American", "small", "large", "national", "young", "different", "black", "long", "little", "important", "political", "bad", "white", "real", "best", "right", "social", "only", "public", "sure", "low", "early", "able", "human", "local", "late", "hard", "major", "better", "economic", "strong", "possible", "whole", "free", "military", "true", "federal", "international", "full", "special", "easy", "clear", "recent", "certain", "personal", "open", "red", "difficult", "available", "likely", "short", "single", "medical", "current", "wrong", "private", "past", "fine", "common", "poor", "natural", "significant", "similar", "hot", "dead", "central", "happy", "serious", "ready", "simple", "left", "physical", "general", "environmental", "financial", "blue", "democratic", "dark", "various", "entire", "close", "legal", "religious", "cold", "final", "main", "green", "nice", "huge", "popular", "traditional", "cultural" ]
