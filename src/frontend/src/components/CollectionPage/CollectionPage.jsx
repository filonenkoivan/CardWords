import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import Card from '../WordCard/Card';
import { Flex } from '@chakra-ui/react';
import Controll from './Controll';
import Quiz from '../Quiz/Quiz';
import { Show } from '@chakra-ui/react';

export default function CollectionPage({ setCollectionId, fetchData }) {
  const { id } = useParams('id');
  const [collection, setCollection] = useState([]);
  const [collectionName, setCollectionName] = useState('');
  const [quizOpen, setQuizOpen] = useState(false);

  const fetchRequest = async () => {
    try {
      const response = await fetch(
        `http://localhost:5268/wordCollection/collections/${id}`,
        {
          method: 'GET',
          credentials: 'include',
        }
      );

      const data = await response.json();

      if (response.ok) {
        setCollection(data.cardList);
        setCollectionName(data.name);
      }
    } catch (error) {
      console.log('server error');
    }
  };

  useEffect(() => {
    fetchRequest();
  }, [fetchData]);

  return (
    <Flex direction={'column'} align={'center'}>
      <h2 style={{ marginBottom: '15px' }}>{collectionName}</h2>
      <Flex wrap={'wrap'} style={{ gap: '15px' }} justify={'center'}>
        {collection?.length > 0 ? (
          collection.map((el) => {
            return (
              <Card
                key={el.id}
                {...el}
                setCollectionId
                collectionId={id}
                fetchRequest={fetchRequest}
              />
            );
          })
        ) : (
          <span>No words here yet — why not create your first one?</span>
        )}
      </Flex>
      <Controll
        setCollectionId={setCollectionId}
        id={id}
        fetchRequest={fetchRequest}
        setQuizOpen={setQuizOpen}
        collection={collection}
      />
      <Quiz
        open={quizOpen}
        setQuizOpen={setQuizOpen}
        id={id}
        collection={collection}
      />
    </Flex>
  );
}
