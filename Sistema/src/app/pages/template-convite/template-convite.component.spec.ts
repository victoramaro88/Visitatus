import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateConviteComponent } from './template-convite.component';

describe('TemplateConviteComponent', () => {
  let component: TemplateConviteComponent;
  let fixture: ComponentFixture<TemplateConviteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TemplateConviteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TemplateConviteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
