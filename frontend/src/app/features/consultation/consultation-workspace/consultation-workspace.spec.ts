import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ConsultationWorkspace } from './consultation-workspace';

describe('ConsultationWorkspace', () => {
  let component: ConsultationWorkspace;
  let fixture: ComponentFixture<ConsultationWorkspace>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConsultationWorkspace],
    }).compileComponents();

    fixture = TestBed.createComponent(ConsultationWorkspace);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
