import React, { useEffect, useState } from 'react';
import { Dialog, Portal, CloseButton, Button } from '@chakra-ui/react';
import styles from './Quiz.module.css';
export default function Quiz({ open, setQuizOpen, id, collection }) {
  const incorrect = new Audio(
    'https://zvukipro.com/uploads/files/2020-11/1604628826_6f578c03d698a4c.mp3'
  );
  const correct = new Audio(
    'https://zvukipro.com/uploads/files/2020-11/1604628556_ffc89ff250028f8.mp3'
  );
  const [quiz, setQuiz] = useState([]);
  const fetchData = async () => {
    if (collection.length >= 3) {
      try {
        const response = await fetch(
          `http://localhost:5268/wordCollection/quiz/${id}`,
          {
            method: 'GET',
            credentials: 'include',
          }
        );
        if (response.ok) {
          const data = await response.json();
          setQuiz(data);
        }
      } catch (error) {
        console.log('server error');
      }
    }
  };
  useEffect(() => {
    fetchData();
  }, [open]);

  function getSiblings(elem) {
    var siblings = [];
    var sibling = elem;
    while (sibling.previousSibling) {
      sibling = sibling.previousSibling;
      sibling.nodeType == 1 && siblings.push(sibling);
    }

    sibling = elem;
    while (sibling.nextSibling) {
      sibling = sibling.nextSibling;
      sibling.nodeType == 1 && siblings.push(sibling);
    }
    siblings = [...siblings, elem];
    return siblings;
  }
  const checkAnswer = (answer, btn) => {
    const btns = getSiblings(btn.target);
    btns.forEach((el) => {
      el.style.pointerEvents = 'none';
    });
    if (answer == quiz.correctAnswer) {
      const sendData = async () => {
        const response = await fetch(
          'http://localhost:5268/wordCollection/updatestats',
          {
            method: 'GET',
            credentials: 'include',
          }
        );
        if (response.ok) {
        }
      };
      sendData();

      correct.play();
      btn.target.style.backgroundColor = 'green';
      setTimeout(() => {
        fetchData();
      }, 1500);
      setTimeout(() => {
        btn.target.style.backgroundColor = 'white';
        btns.forEach((el) => {
          el.style.pointerEvents = 'all';
        });
      }, 1500);
    } else {
      incorrect.play();
      btn.target.style.backgroundColor = 'red';
      setTimeout(() => {
        fetchData();
      }, 4000);
      setTimeout(() => {
        btn.target.style.backgroundColor = 'white';
        btns.forEach((el) => {
          el.style.pointerEvents = 'all';
        });
      }, 4000);
    }
  };
  return (
    <Dialog.Root open={open}>
      <Portal>
        <Dialog.Backdrop />
        <Dialog.Positioner>
          <Dialog.Content>
            <Dialog.Header>
              <Dialog.Title>
                Choose the correct translation: {quiz.word}
              </Dialog.Title>
              <span>{}</span>
            </Dialog.Header>
            <Dialog.Body>
              {quiz?.answers?.map((el, index) => {
                return (
                  <Button
                    key={index}
                    style={{ marginRight: '15px' }}
                    onClick={(btn) => checkAnswer(el, btn)}
                  >
                    {el}
                  </Button>
                );
              })}
            </Dialog.Body>{' '}
            <CloseButton
              style={{ position: 'absolute', top: '10px', right: '10px' }}
              size="sm"
              onClick={() => setQuizOpen(false)}
            />
          </Dialog.Content>
        </Dialog.Positioner>
      </Portal>
    </Dialog.Root>
  );
}
