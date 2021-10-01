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

var books = []book{}

func main() {
	router := gin.Default()
	router.GET("/api/books", getBooks)
	router.GET("/api/books/:id", getBookById)
	router.POST("/api/books", postBook)
	router.DELETE("/api/books/:id", deleteBook)

	router.Run("localhost:5000")
}

func getBooks(c *gin.Context) {
	c.IndentedJSON(http.StatusOK, books)
}

func postBook(c *gin.Context) {
	var newBooks book

	if err := c.BindJSON(&newBooks); err != nil {
		return
	}
	u, _ := uuid.NewV4()
	newBooks.Id = u.String()
	books = append(books, newBooks)
	c.IndentedJSON(http.StatusCreated, newBooks)
}

func getBookById(c *gin.Context) {
	id := c.Param("id")

	for _, a := range books {
		if a.Id == id {
			c.IndentedJSON(http.StatusOK, a)
			return
		}
	}
	c.IndentedJSON(http.StatusNotFound, gin.H{"message": "book not found"})
}

func deleteBook(c *gin.Context) {
	id := c.Param("id")

	for i, a := range books {
		if a.Id == id {
			books[i] = books[len(books)-1]
			books = books[:len(books)-1]
			return
		}
	}
	c.IndentedJSON(http.StatusNotFound, gin.H{"message": "book not found"})
}
