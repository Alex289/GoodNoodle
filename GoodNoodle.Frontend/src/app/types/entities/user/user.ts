import { Guid } from 'guid-typescript';
import { Star } from '../star/star';

export type User = {
  id: Guid;
  email: string;
  fullName: string;
  status: number;
  role: number;
  groupRole: number;
  stars: Star[];
};
