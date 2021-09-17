namespace BookApi.Controllers

open Microsoft.AspNetCore.Mvc
open BookApi
open System

[<ApiController>]
[<Route("api/[controller]")>]
type BookController (bookRepository : BookRepository) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get() =
        bookRepository.Get()

    [<HttpGet("{id}")>]
    member _.GetById(id: Guid) : IActionResult = 
        match bookRepository.GetById(id) with
        | Some book -> new ObjectResult(book) :> IActionResult 
        | None -> NotFoundResult() :> IActionResult 


    [<HttpPost>]
    member _.Post(book: Book) : IActionResult =
        let addedBook = bookRepository.Add(book)
        CreatedAtRouteResult( "/" + addedBook.Id.ToString(), addedBook) :> IActionResult 

    [<HttpDelete("{id}")>]
    member _.Delete(id: Guid) : IActionResult = 
        bookRepository.Delete(id)
        NoContentResult() :> IActionResult

    [<HttpPut("{id}")>]
    member _.Update(id: Guid, book: Book) : IActionResult  =
        bookRepository.Update(id, book)
        NoContentResult() :> IActionResult
