import React, { useEffect, useState, useRef } from 'react';
import '../pages-css/StoreCSS.css';
import { useNavigate } from "react-router-dom";

function Shop() {
    const [eBooks, setEBooks] = useState([]);
    const [filteredEBooks, setFilteredEBooks] = useState([]);
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [searchInput, setSearchInput] = useState('');
    const [searchQuery, setSearchQuery] = useState('');
    const [selectedCategory, setSelectedCategory] = useState(null);
    const categoriesGridRef = useRef(null);

    const navigate = useNavigate();

    // Fetch data on component mount
    useEffect(() => {
        const fetchData = async () => {
            try {
                const [eBooksResponse, categoriesResponse] = await Promise.all([
                    fetch('https://localhost:44332/api/eBook'),
                    fetch('https://localhost:44332/api/Category'),
                ]);

                if (!eBooksResponse.ok || !categoriesResponse.ok) {
                    throw new Error('Failed to fetch data');
                }

                const eBooksData = await eBooksResponse.json();
                const categoriesData = await categoriesResponse.json();

                setEBooks(eBooksData);
                setFilteredEBooks(eBooksData); // Initially display all books
                setCategories(categoriesData);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    // Central filtering logic triggered whenever `searchQuery` or `selectedCategory` changes
    useEffect(() => {
        const filterBooks = async () => {
            let filtered = [...eBooks];

            // Filter by category if one is selected
            if (selectedCategory) {
                try {
                    setLoading(true);
                    const response = await fetch(`https://localhost:44332/api/eBookCategory/byCategory/${selectedCategory}`);
                    if (!response.ok) throw new Error('Failed to fetch books by category');

                    const categoryBooks = await response.json();
                    const transformedBooks = categoryBooks.map(item => item.eBook);
                    filtered = transformedBooks;
                } catch (err) {
                    setError(err.message);
                } finally {
                    setLoading(false);
                }
            }

            // Further filter by search query
            if (searchQuery) {
                const query = searchQuery.toLowerCase();
                filtered = filtered.filter(
                    (ebook) =>
                        ebook.title.toLowerCase().includes(query) ||
                        ebook.author.toLowerCase().includes(query)
                );
            }

            setFilteredEBooks(filtered);
        };

        filterBooks();
    }, [searchQuery, selectedCategory, eBooks]);

    const handleSearch = (event) => {
        event.preventDefault();
        setSearchQuery(searchInput); // Update searchQuery on form submission
    };

    const resetSearch = () => {
        setSearchInput(''); // Clear input field
        setSearchQuery(''); // Clear search query
    };

    const resetCategories = () => {
        setSelectedCategory(null); // Clear selected category
    };

    const scrollCategories = (direction) => {
        if (categoriesGridRef.current) {
            const scrollAmount = 350;
            categoriesGridRef.current.scrollBy({
                left: direction === 'left' ? -scrollAmount : scrollAmount,
                behavior: "smooth",
            });
        }
    };
    const getCategoryNameById = (id) => {
        const category = categories.find((cat) => cat.id === id);
        return category ? category.name : 'Unknown';
    };

    const handleBookClick = (bookId) => {
        navigate(`/ebook/${bookId}`);
    };

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error: {error}</p>;

    return (
        <div>
            <div className="search-container">
                <form className="search-box" onSubmit={handleSearch}>
                    <input
                        type="text"
                        placeholder="Search titles/writers..."
                        className="search-input"
                        value={searchInput}
                        onChange={(e) => setSearchInput(e.target.value)}
                    />
                    <button type="submit" className="search-button">
                        Search
                    </button>
                </form>
            </div>

            {searchQuery && (
                <div className="search-result-text-reset-container">
                    <h3 className="search-result-text">Search Results for: "{searchQuery}"</h3>
                    <a className="reset-button" onClick={resetSearch}>
                        Clear ❌
                    </a>
                </div>
            )}
            {selectedCategory && (
                <div className="search-result-text-reset-container">
                    <h3 className="search-result-text">Category: "{getCategoryNameById(selectedCategory)}"</h3>
                    <a className="reset-button" onClick={resetCategories}>
                        Clear ❌
                    </a>
                </div>
            )}

            <div className="ebook-grid">
                {filteredEBooks.map((ebook) => (
                    <div
                        className="ebook-card"
                        key={ebook.id}
                        onClick={() => handleBookClick(ebook.id)}
                        style={{ cursor: "pointer" }}
                    >
                        <img src={ebook.cover} alt={ebook.title} className="ebook-cover" />
                        <h2>{ebook.title}</h2>
                        <p>By: {ebook.author}</p>
                        <p>Price: ${ebook.price ? ebook.price.toFixed(2) : 'N/A'}</p>
                    </div>
                ))}
            </div>

            <h2>Categories</h2>
            <div className="categories-container">
                <button className="scroll-button left" onClick={() => scrollCategories('left')}>
                    &lt;
                </button>
                <div className="categories-grid" ref={categoriesGridRef}>
                    {categories.map((category) => (
                        <div
                            className={`category-card ${selectedCategory === category.id ? 'selected' : ''}`}
                            key={category.id}
                            onClick={() => setSelectedCategory(category.id)}
                        >
                            <div className="category-image-container">
                                <img src={category.image} alt={category.name} className="category-image" />
                            </div>
                            <p className="category-name">{category.name}</p>
                        </div>
                    ))}
                </div>
                <button className="scroll-button right" onClick={() => scrollCategories('right')}>
                    &gt;
                </button>
            </div>
        </div>
    );
}

export default Shop;
