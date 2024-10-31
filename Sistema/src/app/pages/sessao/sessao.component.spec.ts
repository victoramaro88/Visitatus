import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SessaoComponent } from './sessao.component';

describe('SessaoComponent', () => {
  let component: SessaoComponent;
  let fixture: ComponentFixture<SessaoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SessaoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SessaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
