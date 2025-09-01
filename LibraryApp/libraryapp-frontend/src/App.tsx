import React, { useEffect, useState } from "react";
import { BookDto } from "./types";
import { getBooks, addBook } from "./api/books";
import axios from "axios";

const App: React.FC = () => {
    const [books, setBooks] = useState<BookDto[]>([]);
    const [newTitle, setNewTitle] = useState("");
    const [message, setMessage] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);

    const fetchBooks = async () => {
        try {
            const data = await getBooks();
            setBooks(data);
        } catch {
            setError("Failed to fetch books.");
        }
    };

    const handleAddBook = async () => {
        if (!newTitle.trim()) {
            setError("Book title cannot be empty.");
            return;
        }
        try {
            await addBook(newTitle);
            setNewTitle("");
            setMessage("Book added successfully!");
            fetchBooks();
        } catch {
            setError("Failed to add book.");
        }
    };

    const handleDeleteBook = async (id: string) => {
        try {
            await axios.delete(`http://localhost:8080/api/books/${id}`);
            setMessage("Book deleted successfully!");
            fetchBooks();
        } catch {
            setError("Failed to delete book.");
        }
    };

    const handleToggleBorrow = async (book: BookDto) => {
        try {
            const endpoint = book.isAvailable
                ? `http://localhost:8080/books/${book.id}/borrow`
                : `http://localhost:8080/books/${book.id}/return`;

            await fetch(endpoint, { method: "PUT" });

            setMessage(book.isAvailable ? "Book borrowed!" : "Book returned!");
            fetchBooks();
        } catch {
            setError("Failed to update book status.");
        }
    };

    useEffect(() => {
        fetchBooks();
    }, []);

    return (
        <div className="container py-5">
            <h1 className="text-center mb-4">📚 Library Books</h1>

            {/* Messages */}
            {message && <div className="alert alert-success">{message}</div>}
            {error && <div className="alert alert-danger">{error}</div>}

            {/* Add Book Form */}
            <div className="card shadow-sm mb-4">
                <div className="card-body">
                    <div className="row g-2">
                        <div className="col-md-9">
                            <input
                                type="text"
                                className="form-control"
                                placeholder="Enter a new book title..."
                                value={newTitle}
                                onChange={(e) => {
                                    setNewTitle(e.target.value);
                                    setError(null);
                                }}
                            />
                        </div>
                        <div className="col-md-3 d-grid">
                            <button className="btn btn-primary" onClick={handleAddBook}>
                                ➕ Add Book
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            {/* Books List */}
            <div className="row">
                {books.map((book) => (
                    <div key={book.id} className="col-md-4 mb-3">
                        <div className="card h-100 shadow-sm">
                            <div className="card-body d-flex flex-column justify-content-between">
                                <h5 className="card-title">{book.title}</h5>
                                <span
                                    className={`badge ${book.isAvailable ? "bg-success" : "bg-danger"}`}
                                >
                                    {book.isAvailable ? "Available" : "Borrowed"}
                                </span>
                            </div>
                            <div className="card-footer d-flex justify-content-between">
                                <button
                                    className={`btn btn-sm ${book.isAvailable ? "btn-warning" : "btn-success"}`}
                                    onClick={() => handleToggleBorrow(book)}
                                >
                                    {book.isAvailable ? "Borrow" : "Return"}
                                </button>
                                <button
                                    className="btn btn-sm btn-danger"
                                    onClick={() => handleDeleteBook(book.id)}
                                >
                                    Delete
                                </button>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default App;
