import { Guid } from 'guid-typescript';

export type UserInGroup = {
  id: Guid;
  noodleGroupId: Guid;
  noodleUserId: Guid;
  role: number;
};
