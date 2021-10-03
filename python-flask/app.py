from flask import Flask, request, abort
import uuid

app = Flask(__name__)

books = {}


@app.route("/api/books", methods=['GET'])
def get_books():
    return {
        "books" : list(books.values())
    }


@app.route("/api/books/<uuid:id>", methods=['GET'])
def get_books_by_id(id: uuid):
    if id in books:
        return books[id]
    else:
        return {"message": "book not found"}, 404


@app.route("/api/books", methods=['POST'])
def add_book():
    request_data = request.get_json()
    id = uuid.uuid4()
    book = {
        "id": id,
        "title": request_data["title"],
        "author": request_data["author"],
        "pages": request_data["pages"]
    }
    books[id] = book
    return book


@app.route("/api/books/<uuid:id>", methods=['DELETE'])
def delete_book(id: uuid):
    if id in books:
        del books[id]
        return '', 204
    else:
        return {"message": "book not found"}, 404

