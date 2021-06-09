-- |Lib Documentation starts here.
module Lib
    ( hello,
      addSeries,
      multiplySeries,
      qsort,
      qsortReverse,
      qsortDistinct
    ) where

hello :: IO ()
hello = putStrLn "Hello from Lib"

-------------------------------------------------------------------------------
-- Adds the list of numbers in a series.
--  The additive identity is the base case.
-------------------------------------------------------------------------------

addSeries :: Num p => [p] -> p
addSeries [] = 0
addSeries (n:ns) = n + addSeries ns

-------------------------------------------------------------------------------
-- Multiplies the list of numbers in a series.
--  The multiplicative identity is the base case.
-------------------------------------------------------------------------------

multiplySeries :: Num p => [p] -> p
multiplySeries [] = 1
multiplySeries (n:ns) = n * multiplySeries ns

-------------------------------------------------------------------------------
-- Sorts the list.
-------------------------------------------------------------------------------

qsort :: Ord a => [a] -> [a]
qsort [] = []
qsort (x:xs) = qsort smaller ++ [x] ++ qsort larger
                where
                    smaller = [a | a <- xs, a <= x]
                    larger  = [b | b <- xs, b > x]

-------------------------------------------------------------------------------
-- Sorts the list.
-------------------------------------------------------------------------------

qsortReverse :: Ord a => [a] -> [a]
qsortReverse [] = []
qsortReverse (x:xs) = qsortReverse larger ++ [x] ++ qsortReverse smaller
                        where
                            smaller = [a | a <- xs, a < x]
                            larger  = [b | b <- xs, b >= x]

-------------------------------------------------------------------------------
-- Sorts the list and returns only the distinct values.
-------------------------------------------------------------------------------

qsortDistinct :: Ord a => [a] -> [a]
qsortDistinct [] = []
qsortDistinct (x:xs) = qsortDistinct smaller ++ [x] ++ qsortDistinct larger
                        where
                            smaller = [a | a <- xs, a < x]
                            larger  = [b | b <- xs, b > x]

-------------------------------------------------------------------------------
-- Does a sequence of actions.
-------------------------------------------------------------------------------

-- sequenceActions :: 
sequenceActions []         = return []
sequenceActions (act:acts) = do x <- act
                                xs <- sequenceActions acts
                                return (x:xs)