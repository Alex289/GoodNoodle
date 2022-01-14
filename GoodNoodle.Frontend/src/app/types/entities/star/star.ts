import { Guid } from 'guid-typescript';

export type Star = {
  id: Guid;
  userId: Guid;
  groupId: Guid;
  reason: string;
  beingEdited: boolean;
  shouldDelete: boolean;
};
