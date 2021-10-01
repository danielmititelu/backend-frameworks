namespace BookApi

open System

type Book =
    { Id: Guid
      Title: string
      Author: string
      Pages: int }

type BooksRepository() =
    static let mutable books: Map<Guid, Book> = Map.empty

    member this.Get() =
        books |> Map.toSeq |> Seq.map snd |> Seq.toList

    member this.GetById(id: Guid) =
        let result = books.TryFind(id)
        result

    member this.Add(book: Book) =
        let id = Guid.NewGuid()

        let book =
            { Id = id
              Title = book.Title
              Author = book.Author
              Pages = book.Pages }

        books <- books.Add(id, book)
        book

    member this.Delete(id: Guid) = books <- books.Remove(id)

    member this.Update(id: Guid, book: Book) =
        books <- books.Remove(id)

        let bookToAdd =
            { Id = id
              Title = book.Title
              Author = book.Author
              Pages = book.Pages }

        books <- books.Add(id, bookToAdd)
