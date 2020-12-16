import { TestBed } from '@angular/core/testing';

import { JwtstorageService } from './jwtstorage.service';

describe('JwtstorageService', () => {
  let service: JwtstorageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JwtstorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
