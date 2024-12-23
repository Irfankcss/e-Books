import React, { useEffect, useState, useRef } from 'react';
import '../pages-css/StoreCSS.css';

function Shop() {
    const [eBooks, setEBooks] = useState([]);
    const [filteredEBooks, setFilteredEBooks] = useState([]);
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [searchQuery, setSearchQuery] = useState('');
    const [searchInput, setSearchInput] = useState('');
    const categoriesGridRef = useRef(null);

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
                setFilteredEBooks(eBooksData);
                setCategories(categoriesData);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    const scrollCategories = (direction) => {
        if (categoriesGridRef.current) {
            const scrollAmount = 350;
            categoriesGridRef.current.scrollBy({
                left: direction === 'left' ? -scrollAmount : scrollAmount,
                behavior: "smooth",
            });
        }
    };

    const handleSearch = (event) => {
        event.preventDefault();
        setSearchInput(searchQuery);
        const query = searchQuery.toLowerCase();
        const filtered = eBooks.filter(
            (ebook) =>
                ebook.title.toLowerCase().includes(query) ||
                ebook.author.toLowerCase().includes(query)
        );
        setFilteredEBooks(filtered);
    };
    const resetSearch = () => {
        setSearchQuery('');
        setSearchInput('');
        setFilteredEBooks(eBooks);
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
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                    />
                    <button type="submit" className="search-button">
                        Search
                    </button>
                </form>
            </div>

            {searchInput && <div className="search-result-text-reset-container"><h3>Search Results for: " {searchInput} "</h3>
                <a className="reset-button" onClick={resetSearch}>
                Clear ❌
            </a></div>}

            <div className="ebook-grid">
                {filteredEBooks.map((ebook) => (
                    <div className="ebook-card" key={ebook.id}>
                        <img src={ebook.cover} alt={ebook.title} className="ebook-cover" />
                        <h2>{ebook.title}</h2>
                        <p>By: {ebook.author}</p>
                        <p>Price: ${ebook.price.toFixed(2)}</p>
                        <p>Rating: 4.5</p>
                    </div>
                ))}
            </div>

            <h2>Categories</h2>
            <div className="categories-container">
                <button className="scroll-button left" onClick={() => scrollCategories('left')}>
                    &lt;
                </button>
                <div className="categories-grid" ref={categoriesGridRef}>
                    {categories.map((category, index) => (
                        <div className="category-card" key={index}>
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
