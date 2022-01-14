import { Guid } from 'guid-typescript';

export type InviteToGroup = {
  Id: Guid;
  FullName: string;
  Email: string;
  Role: number;
  GroupId: Guid;
  GroupName: string;
};
