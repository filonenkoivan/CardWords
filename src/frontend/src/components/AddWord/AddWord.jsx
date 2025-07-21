import React, { useEffect } from 'react';
import { useState } from 'react';
import {
  Button,
  CloseButton,
  Dialog,
  Portal,
  Fieldset,
  Input,
  Field,
  Textarea,
} from '@chakra-ui/react';

export default function AddWord({ collectionId, setCollectionId, fetchData }) {
  const [frontSide, setFrontSide] = useState('');
  const [backSide, setBackSide] = useState('');
  const [description, setDescription] = useState('');
  const [translation, setTranslation] = useState('');
  const createNewWord = async (e) => {
    e.preventDefault();

    const data = {
      FrontSideText: frontSide,
      BackSideText: backSide,
      Decsription: description,
    };
    const response = await fetch(
      `http://localhost:5268/wordCollection/collections/words/${collectionId}`,
      {
        method: 'POST',
        body: JSON.stringify(data),
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
      }
    );
    if (response.ok) {
      fetchData();
      setCollectionId(0);
      setBackSide('');
      setFrontSide('');
      setDescription('');
      setTranslation([]);
    }
  };
  async function getTranslation(word) {
    const translateResponse = await fetch(
      `http://localhost:5268/translate?word=${word}`,
      {
        method: 'get',

        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
      }
    );
    const translateData = await translateResponse.json();
    setTranslation(translateData.translations);
  }

  return (
    <div>
      <Dialog.Root open={!!collectionId}>
        <Portal>
          <Dialog.Backdrop />
          <Dialog.Positioner>
            <Dialog.Content>
              <Dialog.Header>
                <Dialog.Title>Create a new card</Dialog.Title>
              </Dialog.Header>
              <form onSubmit={createNewWord}>
                <Dialog.Body>
                  <Fieldset.Root>
                    <Field.Root required>
                      <Field.Label>
                        Front side <Field.RequiredIndicator />
                      </Field.Label>
                      <Input
                        required
                        value={frontSide}
                        onChange={(e) => {
                          setFrontSide(e.target.value);
                          if (e.target.value.length > 3) {
                            getTranslation(e.target.value);
                          } else if (e.target.value.length === 0) {
                            setTranslation([]);
                          }
                        }}
                      />
                    </Field.Root>
                    <Field.Root required>
                      <Field.Label>
                        Back side <Field.RequiredIndicator />
                      </Field.Label>
                      <Input
                        required
                        value={backSide}
                        onChange={(e) => setBackSide(e.target.value)}
                      />
                      <div>
                        <ul>
                          {translation.length > 0 &&
                            translation.map((item, index) => (
                              <li
                                key={index}
                                onClick={() => setBackSide(item)}
                                style={{
                                  cursor: 'pointer',
                                  fontSize: '18px',
                                  padding: '5px 0px',
                                }}
                              >
                                {item}
                              </li>
                            ))}
                        </ul>
                      </div>
                    </Field.Root>
                    <Field.Root>
                      <Field.Label>Description</Field.Label>
                      <Textarea
                        value={description}
                        style={{ resize: 'none' }}
                        onChange={(e) =>
                          description.length < 150 &&
                          setDescription(e.target.value)
                        }
                      />
                    </Field.Root>
                  </Fieldset.Root>
                </Dialog.Body>
                <Dialog.Footer>
                  <Dialog.ActionTrigger asChild>
                    <Button
                      variant="outline"
                      onClick={() => setCollectionId(0)}
                    >
                      Cancel
                    </Button>
                  </Dialog.ActionTrigger>
                  <Button type="submit">Save</Button>
                </Dialog.Footer>
              </form>
              <Dialog.CloseTrigger asChild onClick={() => setCollectionId(0)}>
                <CloseButton size="sm" />
              </Dialog.CloseTrigger>
            </Dialog.Content>
          </Dialog.Positioner>
        </Portal>
      </Dialog.Root>
    </div>
  );
}
