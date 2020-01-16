import { TestBed } from '@angular/core/testing';

import { CountryService } from './country.service';

describe('CountriesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CountryService = TestBed.get(CountryService);
    expect(service).toBeTruthy();
  });
});
