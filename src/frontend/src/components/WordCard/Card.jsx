import React from 'react';
import styles from './Card.module.css';
import { Button, Card, Icon } from '@chakra-ui/react';

export default function WordCard({
  frontSideText,
  backSideText,
  decsription,
  collectionId,
  fetchRequest,
  id,
}) {
  const deleteWord = async () => {
    try {
      const responses = await fetch(
        `http://localhost:5268/wordCollection/collections/${collectionId}/words/${id}`,
        {
          method: 'DELETE',
          credentials: 'include',
        }
      );
      if (responses.ok) {
        fetchRequest();
        console.log('deleted');
      }
    } catch (error) {
      console.log('server error');
    }
  };
  return (
    <div className={styles.card}>
      <Card.Root width="300px" className={styles.cardBack}>
        <Card.Body>
          <Card.Title mt="2">{frontSideText}</Card.Title>
          <Card.Description>{decsription}</Card.Description>
        </Card.Body>
      </Card.Root>
      <Card.Root width="300px" className={styles.cardFront}>
        <Card.Body>
          <Card.Title mt="2">{backSideText}</Card.Title>
        </Card.Body>
      </Card.Root>
      <Button
        className={styles.deletebtn}
        style={{ background: 'transparent', padding: '0' }}
        onClick={() => deleteWord()}
      >
        <Icon>
          <svg
            width="800px"
            height="800px"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M10 12L14 16M14 12L10 16M4 6H20M16 6L15.7294 5.18807C15.4671 4.40125 15.3359 4.00784 15.0927 3.71698C14.8779 3.46013 14.6021 3.26132 14.2905 3.13878C13.9376 3 13.523 3 12.6936 3H11.3064C10.477 3 10.0624 3 9.70951 3.13878C9.39792 3.26132 9.12208 3.46013 8.90729 3.71698C8.66405 4.00784 8.53292 4.40125 8.27064 5.18807L8 6M18 6V16.2C18 17.8802 18 18.7202 17.673 19.362C17.3854 19.9265 16.9265 20.3854 16.362 20.673C15.7202 21 14.8802 21 13.2 21H10.8C9.11984 21 8.27976 21 7.63803 20.673C7.07354 20.3854 6.6146 19.9265 6.32698 19.362C6 18.7202 6 17.8802 6 16.2V6"
              stroke="#ffffff"
              strokeWidth="2"
              strokeLinecap="round"
              strokeLinejoin="round"
            />
          </svg>
        </Icon>
      </Button>
    </div>
  );
}
