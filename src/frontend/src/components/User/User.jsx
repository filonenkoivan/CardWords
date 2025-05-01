import { Flex, HStack } from '@chakra-ui/react';
import React from 'react';
import { Progress } from '@chakra-ui/react';

export default function User({ stats, userVerify }) {
  const logOut = async () => {
    const response = await fetch('http://localhost:5268/auth/logout', {
      method: 'GET',
      credentials: 'include',
    });
    if (response.ok) {
      userVerify(false);
    }
  };
  return (
    <div style={{ paddingLeft: '15px', textAlign: 'left' }}>
      <Flex direction={'column'}>
        <span>Hi, {stats.name}!</span>
        <span>You added {stats.wordsAdded} words</span>
        <span>You learned {stats.wordsLearned} words</span>
        <span style={{ paddingTop: '15px' }}>{stats.rank}</span>
        <Progress.Root
          defaultValue={0}
          maxW="240px"
          value={stats.experience}
          max={
            !!stats?.nextRankExp ? stats.nextRankExp + stats.experience : 100
          }
          min={0}
        >
          <HStack gap="5">
            <Progress.Label>{stats.experience} xp</Progress.Label>
            <Progress.Track flex="1">
              <Progress.Range />
            </Progress.Track>
            <Progress.ValueText>{stats.nextRankExp} xp</Progress.ValueText>
          </HStack>
        </Progress.Root>
        <button
          onClick={() => logOut()}
          style={{ textAlign: 'left', cursor: 'pointer' }}
        >
          Logout
        </button>
      </Flex>
    </div>
  );
}
