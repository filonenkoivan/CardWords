import './App.css';
import { Provider } from './components/ui/provider';
import Login from './components/Login/Login';
import Register from './components/Register/Register';
import Collections from './components/Collections/Collections';
import { useState } from 'react';
import AddWord from './components/AddWord/AddWord';
import User from './components/User/User';
import { Flex } from '@chakra-ui/react';
import { Routes, Route, BrowserRouter } from 'react-router-dom';
import CollectionPage from './components/CollectionPage/CollectionPage';
import PageNotFound from './components/PageNotFound/PageNotFound';
import { Toaster, toaster } from './components/ui/toaster';
function App() {
  const [userState, setUserState] = useState(
    getCookieByName('crumble-cookies')
  );
  const [collectionId, setCollectionId] = useState(0);
  const [collections, setCollections] = useState([]);
  const [userStats, setUserStats] = useState([]);
  function getCookieByName(name) {
    const match = document.cookie.match(
      new RegExp('(^| )' + name + '=([^;]+)')
    );
    if (match) {
      return match[2];
    }
    return null;
  }

  const fetchData = async () => {
    try {
      const response = await fetch(
        'http://localhost:5268/wordCollection/collections',
        {
          method: 'GET',
          credentials: 'include',
        }
      );
      const data = await response.json();
      setCollections(data.collections);
      setUserStats(data.stats);
    } catch (error) {
      console.log('server error');
    }
  };
  return (
    <div className="App">
      <Provider>
        {!userState && (
          <BrowserRouter>
            <Routes>
              <Route
                path="/"
                element={
                  <Login
                    userVerify={setUserState}
                    getCookie={getCookieByName}
                  />
                }
              ></Route>
              <Route path="register" element={<Register />}></Route>
            </Routes>
          </BrowserRouter>
        )}
        <BrowserRouter>
          <Routes>
            <Route
              path="/"
              element={
                <Flex>
                  {userState && (
                    <>
                      <User stats={userStats} userVerify={setUserState} />
                      <Collections
                        setCollectionId={setCollectionId}
                        collections={collections}
                        setCollections={setCollections}
                        fetchData={fetchData}
                      />
                    </>
                  )}
                </Flex>
              }
            ></Route>
            <Route
              path="collections/:id"
              element={
                <CollectionPage
                  setCollectionId={setCollectionId}
                  fetchData={fetchData}
                />
              }
            ></Route>
            <Route path="*" element={<PageNotFound />}></Route>
          </Routes>
        </BrowserRouter>
        <Toaster />
        <AddWord
          collectionId={collectionId}
          setCollectionId={setCollectionId}
          setCollections={setCollections}
          fetchData={fetchData}
        />
      </Provider>
    </div>
  );
}

export default App;
