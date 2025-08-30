import axios from "axios";
import { BookDto } from "../types";

const API_URL = "http://localhost:8080/books"; // replace if needed

export const getBooks = async (): Promise<BookDto[]> => {
    const response = await axios.get<BookDto[]>(API_URL);
    return response.data;
};

export const addBook = async (title: string): Promise<string> => {
    const response = await axios.post<{ bookId: string }>(API_URL, { title });
    return response.data.bookId;
};

// Delete book
export const deleteBook = async (id: number): Promise<void> => {
    await axios.delete(`${API_URL}/${id}`);
};

// Borrow book
export const borrowBook = async (id: number): Promise<void> => {
    await axios.put(`${API_URL}/${id}/borrow`);
};