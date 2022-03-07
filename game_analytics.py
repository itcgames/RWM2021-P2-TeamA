import pandas as pd
from pymongo import MongoClient

db = {}

def connect_to_database():
   client = MongoClient('mongo', 27017, username="test", password="test")
  
   global db
   db = client.test_database
   return db

def connect_to_database_outside_docker():
   client = MongoClient('localhost', 27017, username="test", password="test")

   global db
   db = client.test_database
   return db


def add_data(data):
   posts = db.posts

   post_id = posts.insert_one(data).inserted_id

def get_dataframe():
   cursor = db.posts.find()
   # Expand the cursor and construct the DataFrame
   df = pd.DataFrame(list(cursor))
   return df