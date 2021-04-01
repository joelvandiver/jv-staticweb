import Lib
import Test.Hspec
import Test.QuickCheck
import Control.Exception (evaluate)

main :: IO ()
main = hspec $ do
    describe "addSeries" $ do
        it "should return the additive identity for the base case." $ do
            addSeries [] `shouldBe` (0 :: Int)

        it "should add the numbers in the series." $ do
            addSeries [1,2,3,4,5] `shouldBe` (15 :: Int)

    describe "multiplySeries" $ do
        it "should return the multiplicative identity for the base case." $ do
            multiplySeries [] `shouldBe` (1 :: Int)

        it "should add the numbers in the series." $ do
            multiplySeries [1,2,3,4,5] `shouldBe` (120 :: Int)

    describe "qsort" $ do
        it "should return an empty list from empty list." $ do
            qsort [] `shouldBe` ([] :: [Integer])

        it "should return an ordered list." $ do
            qsort [5,3,2,4,1] `shouldBe` [1,2,3,4,5]

        it "should return duplicates in an ordered list." $ do
            qsort [5,3,2,5,1,2,4,1] `shouldBe` [1,1,2,2,3,4,5,5]

    describe "qsortReverse" $ do
        it "should return an empty list from empty list." $ do
            qsortReverse [] `shouldBe` ([] :: [Integer])

        it "should return an ordered list in reverse." $ do
            qsortReverse [5,3,2,4,1] `shouldBe` [5,4,3,2,1]

        it "should return duplicates in an ordered list in reverse." $ do
            qsortReverse [5,3,2,5,2,4,1,1] `shouldBe` [5,5,4,3,2,2,1,1]

    describe "qsortDistinct" $ do
        it "should return an empty list from empty list." $ do
            qsortDistinct [] `shouldBe` ([] :: [Integer])

        it "should return an ordered list." $ do
            qsortDistinct [5,3,2,4,1] `shouldBe` [1,2,3,4,5]

        it "should not return duplicates in an ordered list." $ do
            qsortDistinct [5,3,2,5,2,4,1] `shouldBe` [1,2,3,4,5]

    -- it "returns the first element of an *arbitrary* list2" $
    --   property $ \x xs -> head (x:xs) == (x :: Int)

    -- it "throws an exception if used with an empty list" $ do
    --   evaluate (head []) `shouldThrow` anyException