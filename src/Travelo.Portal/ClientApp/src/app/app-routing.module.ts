import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CustomersComponent} from './customers/customers.component';
import {CustomerDetailsComponent} from './customer-details/customer-details.component';
import {TripsComponent} from './trips/trips.component';
import {TripDetailsComponent} from './trip-details/trip-details.component';

const routes: Routes = [
    {path: 'customers', component: CustomersComponent},
    {path: 'customers/:id', component: CustomerDetailsComponent},
    {path: 'trips', component: TripsComponent},
    {path: 'trips/:id', component: TripDetailsComponent}
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
