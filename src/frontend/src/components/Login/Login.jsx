import React, { useEffect, useState } from 'react';
import { Field, Fieldset, Input, Button } from '@chakra-ui/react';
import { PasswordInput } from '../ui/password-input';
import { Link } from 'react-router-dom';
import { toaster } from '../ui/toaster';
import { GoogleLogin } from '@react-oauth/google';
import { jwtDecode } from 'jwt-decode';

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
          <Field.Label>Your password</Field.Label>
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
        <div
          style={{
            display: 'flex',
            gap: '25px',
            alignItems: 'center',
            justifyContent: 'space-between',
          }}
        >
          <Link
            to="register"
            style={{ marginTop: '0', fontSize: '14px', textAlign: 'left' }}
          >
            Register
          </Link>

          <div
            style={{
              width: '40px',
              height: '40px',
              overflow: 'hidden',
              borderRadius: '4px',
            }}
          >
            <GoogleLogin
              theme="filled_black"
              type="icon"
              nonce="google-login"
              onSuccess={(credentialResponse) => {
                const user = jwtDecode(credentialResponse.credential);
                let name = user.given_name ?? '' + user.family_name ?? '';
                name = name + Math.floor(Math.random() * 9000);
                const data = {
                  Name: name,
                };
                fetch('http://localhost:5268/auth/google', {
                  method: 'POST',
                  body: JSON.stringify(data),
                  headers: {
                    'Content-Type': 'application/json',
                  },
                  credentials: 'include',
                })
                  .then((response) => response.json())
                  .then((data) => {
                    if (data.success) {
                      userVerify(getCookie('crumble-cookies'));
                    } else {
                      console.log('Google login failed');
                    }
                  })
                  .catch((error) => {
                    console.error('Error:', error);
                  });
              }}
              onError={() => {
                console.log('Login Failed');
              }}
            />
          </div>
        </div>
      </Fieldset.Root>
    </form>
  );
}
