import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateCertificadoComponent } from './template-certificado.component';

describe('TemplateCertificadoComponent', () => {
  let component: TemplateCertificadoComponent;
  let fixture: ComponentFixture<TemplateCertificadoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TemplateCertificadoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TemplateCertificadoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
