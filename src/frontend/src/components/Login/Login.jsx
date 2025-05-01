import React, { useEffect, useState } from 'react';
import { Field, Fieldset, Input, Button } from '@chakra-ui/react';
import { PasswordInput } from '../ui/password-input';
import { Link } from 'react-router-dom';
import { toaster } from '../ui/toaster';

export default function Login({ userVerify, getCookie }) {
  const [nameValue, setNameValue] = useState('');
  const [passwordValue, setPasswordValue] = useState('');

  const sendData = (e) => {
    e.preventDefault();
    const data = {
      name: nameValue,
      password: passwordValue,
    };
    async function fetchData() {
      try {
        const submitBtn = document.querySelector('.submitBtn');
        submitBtn.disabled = true;
        const response = fetch('http://localhost:5268/auth/login', {
          method: 'POST',
          body: JSON.stringify(data),
          headers: {
            'Content-Type': 'application/json',
          },
          credentials: 'include',
        });

        toaster.promise(response, {
          success: {
            title: 'You have successfully logged in',
            description: 'Looks great!',
          },
          error: {
            title: 'Login error!',
            description: 'Incorrect username or password. Please try again.',
          },
          loading: {
            title: 'We are looking for your account...',
            description: 'Please wait',
          },
        });

        const result = await response;
        if (result.ok) {
          console.log('loggined!');
          userVerify(getCookie('crumble-cookies'));
        } else {
          submitBtn.disabled = false;
          console.log('invalid info');
        }
      } catch (ex) {
        console.log('server exception');
      }
    }
    fetchData();
  };
  return (
    <form onSubmit={sendData}>
      <Fieldset.Root size="md" style={{ width: '50%', margin: '0 auto' }}>
        <Fieldset.Legend>Login</Fieldset.Legend>
        <Field.Root required>
          <Field.Label>
            {/* <Field.RequiredIndicator /> */}
            Your name
          </Field.Label>
          <Input
            name="name"
            type="text"
            value={nameValue}
            onChange={(e) => setNameValue(e.target.value)}
          />
          <Field.HelperText>
            Name must be at least 5 characters long
          </Field.HelperText>
        </Field.Root>
        <Field.Root required>
          <Field.Label>
            {/* <Field.RequiredIndicator /> */}
            Your password
          </Field.Label>
          <PasswordInput
            value={passwordValue}
            onChange={(e) => setPasswordValue(e.target.value)}
          />
        </Field.Root>
        <Button type="submit" className="submitBtn">
          Submit
        </Button>
        <Fieldset.Content style={{ marginTop: '5px' }} />
        <Fieldset.ErrorText>
          Some fields are invalid. Please check them.
        </Fieldset.ErrorText>
        <Link
          to="register"
          style={{ marginTop: '0', fontSize: '14px', textAlign: 'left' }}
        >
          Register
        </Link>
      </Fieldset.Root>
    </form>
  );
}
