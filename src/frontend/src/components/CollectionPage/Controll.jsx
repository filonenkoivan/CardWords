import apiUrl from '../../environment/environment';
import { Button } from '@chakra-ui/react';
import { jwtDecode } from 'jwt-decode';
import getCookieByName from '../../functions/getCookieByName';
import { toaster } from '../ui/toaster';
import styles from './Controll.module.css';
export default function Controll({
  setCollectionId,
  id,
  setQuizOpen,
  collection,
  userId,
}) {
  const isCollectionCreator = () => {
    let userIdFromToken = jwtDecode(getCookieByName('crumble-cookies')).userId;
    return userId === parseInt(userIdFromToken);
  };
  const saveCollection = async () => {
    const response = fetch(`${apiUrl}community/collections/save?id=${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${getCookieByName('crumble-cookies')}`,
      },
    });

    toaster.promise(response, {
      success: {
        title: 'Saved!',
        description: 'You can find it in your profile.',
      },
      error: {
        title: 'You already saved this collection',
        description: 'You can find it in your profile.',
      },
      loading: { title: 'Saving...' },
    });
  };
  return (
    <div style={{ position: 'fixed', bottom: '15px', right: '15px' }}>
      {isCollectionCreator() && collection.length >= 3 && (
        <Button
          onClick={() => {
            setQuizOpen(true);
          }}
        >
          Play
        </Button>
      )}
      {isCollectionCreator() && (
        <Button
          style={{ backgroundColor: 'transparent' }}
          onClick={() => {
            setCollectionId(id);
          }}
        >
          <svg
            style={{ width: '43px', height: '62px' }}
            width="43px"
            height="62px"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M11 8C11 7.44772 11.4477 7 12 7C12.5523 7 13 7.44771 13 8V11H16C16.5523 11 17 11.4477 17 12C17 12.5523 16.5523 13 16 13H13V16C13 16.5523 12.5523 17 12 17C11.4477 17 11 16.5523 11 16V13H8C7.44772 13 7 12.5523 7 12C7 11.4477 7.44771 11 8 11H11V8Z"
              fill="#ffffff"
            />
            <path
              fillRule="evenodd"
              clipRule="evenodd"
              d="M23 4C23 2.34315 21.6569 1 20 1H4C2.34315 1 1 2.34315 1 4V20C1 21.6569 2.34315 23 4 23H20C21.6569 23 23 21.6569 23 20V4ZM21 4C21 3.44772 20.5523 3 20 3H4C3.44772 3 3 3.44772 3 4V20C3 20.5523 3.44772 21 4 21H20C20.5523 21 21 20.5523 21 20V4Z"
              fill="#ffffff"
            />
          </svg>
        </Button>
      )}
      {!isCollectionCreator() && (
        <Button className={styles.controllSave} onClick={saveCollection}>
          <svg
            style={{ width: '50px', height: '50px' }}
            xmlns="http://www.w3.org/2000/svg"
            x="0px"
            y="0px"
            viewBox="0,0,256,256"
          >
            <g
              fill="#ffffff"
              fill-rule="nonzero"
              stroke="none"
              stroke-width="1"
              stroke-linecap="butt"
              stroke-linejoin="miter"
              stroke-miterlimit="10"
              stroke-dasharray=""
              stroke-dashoffset="0"
              font-family="none"
              font-weight="none"
              font-size="none"
              text-anchor="none"
              style={{ mixBlendMode: 'normal' }}
            >
              <g transform="scale(8.53333,8.53333)">
                <path d="M23,27l-8,-7l-8,7v-22c0,-1.105 0.895,-2 2,-2h12c1.105,0 2,0.895 2,2z"></path>
              </g>
            </g>
          </svg>
        </Button>
      )}
    </div>
  );
}
