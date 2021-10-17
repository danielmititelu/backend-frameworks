#[macro_use]
extern crate rocket;
use rocket::response::status::NotFound;
use rocket::serde::json::Json;
use rocket::serde::{Deserialize, Serialize};
use rocket::tokio::sync::Mutex;
use rocket::State;
use std::collections::HashMap;
use uuid::Uuid;

#[derive(Serialize, Deserialize, Clone)]
#[serde(crate = "rocket::serde")]
struct Book {
    #[serde(skip_deserializing)]
    id: Uuid,
    title: String,
    author: String,
    pages: u8,
}

#[derive(Serialize, Deserialize, Clone)]
#[serde(crate = "rocket::serde")]
struct Message {
    message: String
}

type BookList = Mutex<HashMap<Uuid, Book>>;
type Books<'r> = &'r State<BookList>;

#[get("/books")]
async fn get_books(books: Books<'_>) -> Json<Vec<Book>> {
    let books = books.lock().await;
    let books = books.values().cloned().collect();
    Json(books)
}

#[post("/books", format = "json", data = "<book>")]
async fn post_book(book: Json<Book>, books: Books<'_>) -> Json<Book> {
    let mut books = books.lock().await;
    let id = Uuid::new_v4();
    books.insert(
        id,
        Book {
            id,
            title: book.title.to_string(),
            author: book.author.to_string(),
            pages: book.pages,
        },
    );

    Json(Book {
        id,
        title: book.title.to_string(),
        author: book.author.to_string(),
        pages: book.pages,
    })
}

#[get("/books/<id>", format = "json")]
async fn get_book_by_id(id: Uuid, books: Books<'_>) -> Result<Json<Book>, NotFound<Json<Message>>> {
    let books = books.lock().await;
    books.get(&id)
        .map(|b| Json(b.clone()))
        .ok_or_else(|| NotFound(Json(Message {message: format!("Missing book for UUID: {}", id)})))
}

#[delete("/books/<id>")]
async fn delete_book(id: Uuid, books: Books<'_>) -> Result<Json<Message>, NotFound<Json<Message>>> {
    let mut books = books.lock().await;
    books.remove(&id)
        .map(|_b| Json(Message {message: format!("Deleted book for UUID: {}", id)}))
        .ok_or_else(|| NotFound(Json(Message {message: format!("Missing book for UUID: {}", id)})))
}

#[launch]
fn rocket() -> _ {
    rocket::build()
        .manage(BookList::new(HashMap::new()))
        .mount("/api", routes![get_books, post_book, get_book_by_id, delete_book])
}
