package main

import (
	"net/http"

	"github.com/gin-gonic/gin"

	uuid "github.com/nu7hatch/gouuid"
)

type book struct {
	Id     string  `json:"id"`
	Title  string  `json:"title"`
	Author string  `json:"author"`
	Pages  float64 `json:"pages"`
}

var books = make(map[string]book)

func main() {
	router := gin.Default()
	router.GET("/api/books", getBooks)
	router.GET("/api/books/:id", getBookById)
	router.POST("/api/books", postBook)
	router.DELETE("/api/books/:id", deleteBook)

	router.Run("localhost:5000")
}

func getBooks(c *gin.Context) {
	values := make([]book, 0, len(books))

	for _, book := range books {
		values = append(values, book)
	}
	c.IndentedJSON(http.StatusOK, values)
}

func postBook(c *gin.Context) {
	var newBook book

	if err := c.BindJSON(&newBook); err != nil {
		return
	}
	id, _ := uuid.NewV4()
	newBook.Id = id.String()
	books[newBook.Id] = newBook
	c.IndentedJSON(http.StatusCreated, newBook)
}

func getBookById(c *gin.Context) {
	id := c.Param("id")

	book, ok := books[id]
	if !ok {
		c.IndentedJSON(http.StatusNotFound, gin.H{"message": "book not found"})
		return
	}
	c.IndentedJSON(http.StatusOK, book)
}

func deleteBook(c *gin.Context) {
	id := c.Param("id")

	_, ok := books[id]
	if !ok {
		c.IndentedJSON(http.StatusNotFound, gin.H{"message": "book not found"})
		return
	}

	delete(books, id)
	c.IndentedJSON(http.StatusNoContent, gin.H{"message": "book deleted"})
}
