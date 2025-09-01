// src/App.test.tsx
import React from "react";
import { render, screen, fireEvent, waitFor, within } from "@testing-library/react";

import App from "./App";
import { BookDto } from "./types";
import axios from "axios";
jest.mock("axios")
const mockedAxios = axios as jest.Mocked<typeof axios>;

describe("📚 App Component", () => {
    const sampleBooks: BookDto[] = [
        { id: "1", title: "Book A", isAvailable: true },
        { id: "2", title: "Book B", isAvailable: false },
    ];

    beforeEach(() => {
        jest.clearAllMocks();
    });

    test("loads and displays books", async () => {
        mockedAxios.get.mockResolvedValueOnce({ data: sampleBooks });

        render(<App />);

        // wait for books to show up
        expect(await screen.findByText("Book A")).toBeInTheDocument();
        expect(screen.getByText("Book B")).toBeInTheDocument();
    });

    test("adds a new book", async () => {
        mockedAxios.get.mockResolvedValueOnce({ data: [] }); // initial fetch
        mockedAxios.post.mockResolvedValueOnce({ data: { bookId: "3" } });
        mockedAxios.get.mockResolvedValueOnce({
            data: [...sampleBooks, { id: "3", title: "Book C", isAvailable: true }],
        }); // after adding

        render(<App />);

        fireEvent.change(screen.getByPlaceholderText(/Enter a new book title/i), {
            target: { value: "Book C" },
        });
        fireEvent.click(screen.getByText(/Add Book/i));

        expect(await screen.findByText("Book C")).toBeInTheDocument();
    });

    test("deletes a book", async () => {
        // 1️⃣ Initial load: two books
        mockedAxios.get.mockResolvedValueOnce({
            data: [
                { id: "10", title: "Book A", isAvailable: true },
                { id: "20", title: "Book B", isAvailable: false },
            ],
        });

        // 2️⃣ Mock delete success
        mockedAxios.delete.mockResolvedValueOnce({});

        // 3️⃣ After deletion: only Book B remains
        mockedAxios.get.mockResolvedValueOnce({
            data: [{ id: "20", title: "Book B", isAvailable: false }],
        });

        render(<App />);

        // Wait for "Book A" to appear
        const bookAHeading = await screen.findByRole("heading", { name: "Book A" });

        // Get the delete button that belongs to the same card as "Book A"
        const deleteButton = within(bookAHeading.closest("div.card") as HTMLElement)
            .getByRole("button", { name: /delete/i });

        // Click delete
        fireEvent.click(deleteButton);

        // Assert Book A disappears
        await waitFor(() =>
            expect(screen.queryByRole("heading", { name: "Book A" })).not.toBeInTheDocument()
        );

        // Assert success message appears
        expect(await screen.findByText("Book deleted successfully!")).toBeInTheDocument();
    });

});
