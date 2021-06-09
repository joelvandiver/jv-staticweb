module Main where

import System.Environment()
import System.IO()

main :: IO ()
main = do
    putStrLn "Hi, what is your name?"
    name <- getLine
    putStrLn ("Hey, " ++ name ++ ", it's nice to meet you!")
    putStrLn $ "Where do you dream of going to?"
    dream <- getLine
    putStrLn $ "Someday you're going to go to " ++ dream ++ "!"
