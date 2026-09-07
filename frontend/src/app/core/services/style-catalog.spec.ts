import { TestBed } from '@angular/core/testing';
import { StyleCatalog } from './style-catalog';

describe('StyleCatalog', () => {
  let service: StyleCatalog;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StyleCatalog);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
