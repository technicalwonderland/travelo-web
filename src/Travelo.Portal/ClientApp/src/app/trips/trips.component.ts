import {Component, OnInit} from '@angular/core';
import {TripService} from '../services/trip.service';
import {Trip, TripStatus} from '../models/trip';
import {Country} from '../models/country';
import {CountryService} from '../country.service';
import {MatDatepickerInputEvent, MatSelectionListChange} from '@angular/material';

@Component({
    selector: 'app-trips',
    templateUrl: './trips.component.html',
    styleUrls: ['./trips.component.css']
})
export class TripsComponent implements OnInit {

    public trips: Trip[];
    public countries: Country[];
    public selectedDestination: Country;
    public selectedName: string;
    tomorrow = new Date();
    maxDate = new Date();
    selectedStartDate: Date;
    selectedEdDate: Date;

    constructor(private tripService: TripService, private countryService: CountryService) {
        this.tomorrow.setDate(this.tomorrow.getDate() + 1);
        this.maxDate.setDate(this.tomorrow.getDate() + 30);
    }

    ngOnInit() {
        this.getTrips();
        this.countries = this.countryService.getCountries();
    }

    private getTrips(): void {
        this.tripService.getTrips().subscribe(trips => this.trips = trips);
    }

    addTrip() {
        const trip = new Trip();
        trip.startDate = this.selectedStartDate.toDateString();
        trip.endDate = this.selectedEdDate.toDateString();
        trip.tripStatus = TripStatus.Active;
        trip.name = this.selectedName;
        trip.destination = this.selectedDestination.name;
        JSON.stringify(trip);
        this.tripService.addTrip(trip).subscribe(() => this.trips.push(trip));
    }

    setStartDate($event: MatDatepickerInputEvent<Date>) {
        this.selectedStartDate = $event.value;
    }

    setEndDate($event: MatDatepickerInputEvent<Date>) {
        this.selectedEdDate = $event.value;
    }

}
