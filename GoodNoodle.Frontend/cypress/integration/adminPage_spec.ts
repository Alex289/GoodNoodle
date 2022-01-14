describe('Admin page Tests', () => {
  it('Should visit the login page', () => {
    cy.visit('/login');
    cy.contains('Login');
  });

  it('Should login user', () => {
    cy.get('#input-email').type('goodnoodle.noreply@gmail.com');
    cy.get('#input-password').type('!Password123#');
    cy.get('button[status="primary"]').contains('Log In').click();
  });

  it('Should have logged in user', () => {
    cy.get('nb-user').should('exist');
    cy.get('.user-name').contains('Admin').should('be.visible');
    cy.visit('/admin');
  });
});
