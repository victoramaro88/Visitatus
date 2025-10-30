import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CertificadoComponent } from './certificado.component';

describe('CertificadoComponent', () => {
  let component: CertificadoComponent;
  let fixture: ComponentFixture<CertificadoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CertificadoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CertificadoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
