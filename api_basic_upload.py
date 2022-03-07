from flask import Flask, request, Blueprint, make_response
import json
import datetime
import game_analytics

api_basic_upload = Blueprint('api_basic_upload', __name__)

@api_basic_upload.route('/upload_data', methods=['POST'])
def post_upload():
 try:
   data = request.get_json()
   data['timestamp'] = datetime.datetime.utcnow()
   print(data)
   game_analytics.connect_to_database()
   game_analytics.add_data(data)
   return "{'status': 'success'}"
 except Exception as e:
   print(e)
   resp = make_response(str(e), 500)
   return resp