import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConviteComponent } from './convite.component';

describe('ConviteComponent', () => {
  let component: ConviteComponent;
  let fixture: ComponentFixture<ConviteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConviteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConviteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
