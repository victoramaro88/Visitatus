import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GestaoAdministrativaComponent } from './gestao-administrativa.component';

describe('GestaoAdministrativaComponent', () => {
  let component: GestaoAdministrativaComponent;
  let fixture: ComponentFixture<GestaoAdministrativaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GestaoAdministrativaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GestaoAdministrativaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
