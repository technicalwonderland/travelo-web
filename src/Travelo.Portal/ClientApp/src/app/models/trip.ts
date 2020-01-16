import {Customer} from './customer';
import {Country} from './country';

export enum TripStatus {
    Active = 'Active',
    Cancelled = 'Cancelled'
}

export class Trip {
    id: string;
    startDate: string;
    endDate: string;
    tripStatus: TripStatus;
    customers: Customer[];
    destination: string;
    name: string;
}
