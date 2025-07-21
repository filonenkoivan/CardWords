import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Input } from '@chakra-ui/react';
import apiUrl from '../../environment/environment';

export default function Search() {
  const [searchValue, setSearchValue] = useState('');
  const [searchResults, setSearchResults] = useState([]);
  const navigate = useNavigate();

  const handleSearch = async (e) => {
    e.preventDefault();
    if (!searchValue.trim()) return;

    try {
      const response = await fetch(
        `${apiUrl}community/collections?name=${encodeURIComponent(
          e.target.value
        )}`,
        {
          method: 'GET',
          credentials: 'include',
          headers: {
            'Content-Type': 'application/json',
          },
        }
      );

      if (!response.ok) {
        console.log(response);
        setSearchResults([]);
        return;
      }

      const results = await response.json();
      setSearchResults(results);
    } catch (error) {
      console.error('Error fetching search results:', error);
    }
  };
  const navigateToCollection = (e, id) => {
    if (!e.target.closest('.actionBtn')) {
      navigate(`collections/${id}`);
    }
  };
  return (
    <div className="search-component">
      <form>
        <Input
          type="text"
          value={searchValue}
          style={{
            paddingLeft: '10px',
            borderRadius: '5px',

            marginBottom: '10px',
          }}
          onChange={(e) => {
            if (e.target.value === '') {
              setSearchResults([]);
            }
            handleSearch(e);

            setSearchValue(e.target.value);
          }}
          placeholder="Search collections..."
        />
      </form>
      <ul>
        {searchResults?.map((result, index) => (
          <li
            onClick={(e) => navigateToCollection(e, result.id)}
            key={index}
            style={{
              cursor: 'pointer',
              borderBottom: '1px solid #eee',
              paddingBottom: '10px',
            }}
          >
            <div>{result.name}</div>
            <div
              style={{
                maxWidth: '180px',
                display: 'inline-flex',
                marginTop: '5px',
                gap: '5px',
                flexWrap: 'wrap',
              }}
            >
              {result.cardList?.map((card, cardIndex) => (
                <div
                  key={cardIndex}
                  style={{
                    backgroundColor: 'white',
                    padding: '0px 5px',
                    borderRadius: '5px',
                    color: 'black',
                    fontSize: '14px',
                  }}
                >
                  {card.frontSideText}
                </div>
              ))}
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}
