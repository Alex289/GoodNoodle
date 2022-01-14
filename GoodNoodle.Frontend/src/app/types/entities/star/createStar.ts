import { Guid } from 'guid-typescript';

export type CreateStar = {
  userId: Guid;
  groupId: Guid;
  reason: string;
};
