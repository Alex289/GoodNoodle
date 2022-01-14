describe('Register page Tests', () => {
  it('Should visit the register page', () => {
    cy.visit('/register');
    cy.contains('Register');
  });

  it('Should register user', () => {
    cy.get('#input-name').type('Cypress Register');
    cy.get('#input-email').type(
      `cypress-${Math.floor(Math.random() * 90000) + 10000}@register.com`
    );
    cy.get('#input-password').type('Cypress1#');
    cy.get('#input-re-password').type('Cypress1#');
    cy.get('.custom-checkbox').click();
    cy.get('button[status="primary"]').contains('Register').click();
  });

  it('Should have registered user', () => {
    cy.get('nb-user').should('exist');
    cy.get('.user-name').contains('Cypress Register').should('be.visible');
  });
});
