import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import Card from '../WordCard/Card';
import { Flex } from '@chakra-ui/react';
import Controll from './Controll';
import Quiz from '../Quiz/Quiz';
import apiUrl from '../../environment/environment';

export default function CollectionPage({ setCollectionId, fetchData }) {
  const { id } = useParams('id');
  const [collection, setCollection] = useState([]);
  const [userId, setUserId] = useState(0);
  const [collectionName, setCollectionName] = useState('');
  const [quizOpen, setQuizOpen] = useState(false);

  const fetchRequest = async () => {
    try {
      const response = await fetch(
        `${apiUrl}wordCollection/collections/${id}`,
        {
          method: 'GET',
          credentials: 'include',
        }
      );

      const data = await response.json();
      console.log(data);
      if (response.ok) {
        setCollection(data.cardList);
        setCollectionName(data.name);
        setUserId(data.userId);
      }
    } catch (error) {
      console.log(error);
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
        userId={userId}
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
