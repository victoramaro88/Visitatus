import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PresencaSessaoComponent } from './presenca-sessao.component';

describe('PresencaSessaoComponent', () => {
  let component: PresencaSessaoComponent;
  let fixture: ComponentFixture<PresencaSessaoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PresencaSessaoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PresencaSessaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
