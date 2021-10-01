namespace BookApi.Controllers

open Microsoft.AspNetCore.Mvc
open BookApi
open System

[<ApiController>]
[<Route("api/[controller]")>]
type BooksController(booksRepository: BooksRepository) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get() = booksRepository.Get()

    [<HttpGet("{id}")>]
    member _.GetById(id: Guid) : IActionResult =
        match booksRepository.GetById(id) with
        | Some book -> new ObjectResult(book) :> IActionResult
        | None -> NotFoundResult() :> IActionResult


    [<HttpPost>]
    member _.Post(book: Book) : IActionResult =
        let addedBook = booksRepository.Add(book)
        CreatedAtRouteResult("/" + addedBook.Id.ToString(), addedBook) :> IActionResult

    [<HttpDelete("{id}")>]
    member _.Delete(id: Guid) : IActionResult =
        booksRepository.Delete(id)
        NoContentResult() :> IActionResult

    [<HttpPut("{id}")>]
    member _.Update(id: Guid, book: Book) : IActionResult =
        booksRepository.Update(id, book)
        NoContentResult() :> IActionResult
