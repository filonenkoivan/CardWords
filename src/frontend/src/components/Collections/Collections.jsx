import React, { useEffect, useState } from 'react';
import { Field, Fieldset, Input, Button } from '@chakra-ui/react';
import Collection from './Collection';
import { toaster } from '../ui/toaster';

export default function Collections({
  setCollectionId,
  collections,
  fetchData,
}) {
  const [collectionName, setCollectionName] = useState('');

  useEffect(() => {
    fetchData();
  }, []);
  const createCollection = (e) => {
    e.preventDefault();
    const data = {
      name: collectionName,
    };
    const sendData = async () => {
      try {
        const response = await fetch(
          'http://localhost:5268/wordCollection/collections',
          {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
            },
            credentials: 'include',
            body: JSON.stringify(data),
          }
        );
        if (response.ok) {
          fetchData();
          setCollectionName('');
          toaster.create({
            description: 'Collection created',
            type: 'success',
            duration: 2000,
          });
        } else if (response.status == 409) {
          toaster.create({
            description: "You've already created a collection with that name.",
            type: 'error',
            duration: 2000,
          });
        }
      } catch (error) {
        console.log('server error');
      }
    };
    sendData();
  };
  return (
    <form onSubmit={createCollection} style={{ flexGrow: '1' }}>
      <Fieldset.Root
        style={{ width: '50%', margin: '0 auto', marginBottom: '15px' }}
      >
        <Field.Root>
          <Field.Label>Create a new collection of words</Field.Label>
          <Field.Root required orientation={'horizontal'}>
            <Input
              placeholder="Collection name"
              value={collectionName}
              onChange={(e) => setCollectionName(e.target.value)}
            />
            <Button type="submit">Create</Button>
          </Field.Root>
        </Field.Root>
      </Fieldset.Root>
      <div className="collectionList">
        {collections.length != 0
          ? collections.map((el) => {
              return (
                <Collection
                  wordsList={el.cardList}
                  createdTime={el.createdTime}
                  key={el.id}
                  render={fetchData}
                  id={el.id}
                  setCollectionId={setCollectionId}
                >
                  {el.name}
                </Collection>
              );
            })
          : 'No collections have been created yet.'}
      </div>
    </form>
  );
}
