import React, { useEffect, useState } from 'react';
import { Field, Fieldset, Input, Button } from '@chakra-ui/react';
import { PasswordInput } from '../ui/password-input';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { toaster } from '../ui/toaster';
export default function Register({ userVerify, getCookie }) {
  const [nameValue, setNameValue] = useState('');
  const [passwordValue, setPasswordValue] = useState('');
  const [passwordConfirmValue, setPasswordConfirmValue] = useState('');
  const navigate = useNavigate();
  const sendData = (e) => {
    e.preventDefault();
    const data = {
      name: nameValue,
      password: passwordValue,
      passwordConfirm: passwordConfirmValue,
    };
    async function fetchData() {
      try {
        const submitButton = document.querySelector('.submitButton');
        console.log(submitButton);
        submitButton.disabled = true;
        const response = fetch('http://localhost:5268/auth/register', {
          method: 'POST',
          body: JSON.stringify(data),
          headers: {
            'Content-Type': 'application/json',
          },
          credentials: 'include',
        });
        toaster.promise(response, {
          success: {
            title: 'User created!',
            description: 'Looks great',
          },
          error: {
            title: 'User creation failed',
            description: 'Something wrong with the data',
          },
          loading: { title: 'User creating...', description: 'Please wait' },
        });

        const result = await response;
        const errorResult = await result.json();
        if (result.ok) {
          navigate('../');
        } else if (result.status === 409) {
          submitButton.disabled = false;
          toaster.create({
            title: 'Error!',
            description: 'This username is already taken.',
          });
        } else {
          submitButton.disabled = false;
          errorResult.forEach((el) => {
            toaster.create({
              title: 'Error!',
              description: `${el.message}`,
            });
          });
        }
      } catch (ex) {
        console.log(ex);
      }
    }
    fetchData();
  };
  return (
    <form onSubmit={sendData}>
      <Fieldset.Root size="md" style={{ width: '50%', margin: '0 auto' }}>
        <Fieldset.Legend>Register</Fieldset.Legend>
        <Field.Root required>
          <Field.Label>Your name</Field.Label>
          <Input
            name="name"
            type="text"
            value={nameValue}
            onChange={(e) => setNameValue(e.target.value)}
            required
          />
          <Field.HelperText>
            Name must be at least 5 characters long
          </Field.HelperText>
        </Field.Root>
        <Field.Root required>
          <Field.Label>Your password</Field.Label>
          <PasswordInput
            value={passwordValue}
            onChange={(e) => setPasswordValue(e.target.value)}
            required
          />
          <Field.HelperText>
            Password must be at least 5 characters long
          </Field.HelperText>
        </Field.Root>
        <Field.Root required>
          <Field.Label>Confirm your password</Field.Label>
          <PasswordInput
            value={passwordConfirmValue}
            onChange={(e) => setPasswordConfirmValue(e.target.value)}
            required
          />
        </Field.Root>
        <Button type="submit" className="submitButton">
          Submit
        </Button>
        <Fieldset.Content style={{ marginTop: '5px' }} />
        <Fieldset.ErrorText>
          Some fields are invalid. Please check them.
        </Fieldset.ErrorText>
        <Link
          to="../"
          style={{ marginTop: '0', fontSize: '14px', textAlign: 'left' }}
        >
          Login
        </Link>
      </Fieldset.Root>
    </form>
  );
}
